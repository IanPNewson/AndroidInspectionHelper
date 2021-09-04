package com.androidteststudio.helper

import android.graphics.Bitmap
import com.google.gson.GsonBuilder
import com.google.gson.TypeAdapter
import com.google.gson.stream.JsonReader
import com.google.gson.stream.JsonWriter
import fi.iki.elonen.NanoHTTPD
import java.io.ByteArrayInputStream
import java.io.ByteArrayOutputStream
import java.io.File
import java.nio.file.Files
import java.nio.file.Paths
import java.security.PublicKey
import java.security.spec.X509EncodedKeySpec
import java.util.*
import javax.crypto.Cipher

class WebApp(val responder :Responder, port: Int) : NanoHTTPD(port) {

    @Suppress("SpellCheckingInspection")
    override fun serve(
        _uri: String?,
        method: Method?,
        headers: MutableMap<String, String>?,
        parms: MutableMap<String, String>?,
        files: MutableMap<String, String>?
    ): Response {
        val gson = GsonBuilder()
            .registerTypeAdapter(Boolean::class.javaObjectType, BoolIgnoreFalseAdapter())
            .create()

        val uri = _uri?.lowercase(Locale.getDefault())
        val pathParts = uri
            ?.split('/')
            ?.filter { it.isNotBlank() }

        try {
            when (pathParts?.get(0)) {
                "isalive" -> return text("true")
                "getpackages" -> {
                    val packages = this.responder.getPackageList();
                    val json = gson.toJson(packages)
                    return json(json)
                }
                "getpackageicon" -> {
                    val download = parms?.get("download")?.toBoolean()?:false

                    val packageId = pathParts.let {
                        if (it.size >= 2 &&
                            it[1].isNotBlank()) it[1]
                        else
                            null
                    } ?:
                        parms?.get("packageId")
                        ?: return badRequest("No packageId provided")

                    val bmp = responder.getPackageIcon(packageId)
                        ?: return notFound("No icon found for packageId '$packageId'")

                    val os = ByteArrayOutputStream()
                    bmp.compress(Bitmap.CompressFormat.PNG, 100, os)
                    val bytes = os.toByteArray()
                    return data(bytes, "image/png")
                        .let {
                             if (download) it.downloadAs("$packageId.icon.png") else it
                        }
                }
                "getviews" -> {
                    val encryptionKeyPath = parms?.get("encryptionKeyPath");
                    var publicKey :PublicKey? = null
                    if (BuildConfig.ENABLE_HTTP_CONTENT_ENCRYPTION) {
                        if (encryptionKeyPath == null || encryptionKeyPath.isBlank())
                            throw IllegalArgumentException("encryptionKeyPath must be provided via the query string")
                        publicKey = PublicKeyReader.get(encryptionKeyPath)
                    } else {
                        publicKey = encryptionKeyPath?.let { PublicKeyReader.get(it) }
                    }

                    val views = responder.getViews();
                    val json = gson.toJson(views)
                        .replace("\"children\":[],", "")
                    if (publicKey != null) {
                        val cipher = Cipher.getInstance("RSA/ECB/OAEPWithSHA1AndMGF1Padding")
                        cipher.init(Cipher.ENCRYPT_MODE, publicKey)
                        val encrypted = cipher.doFinal(json.toByteArray())
                        return data(encrypted, "encrypted/json").apply {
                            addHeader("X-ENCRYPTION-KEY-PATH", encryptionKeyPath)
                        }
                    } else{
                        return json(json)
                    }
                }
            }
        } catch (ex :Exception) {
            return internalError(ex)
        }

        return notFound("URL '$uri' is not supported")
    }

    //region encryption

    object PublicKeyReader {
        @kotlin.Throws(Exception::class)
        operator fun get(filename: String): PublicKey {
            val key = File(filename).readText()
                .replace("-", "")
                .replace("\n", "")
                .replace("\r", "")
                .replace("BEGIN PUBLIC KEY", "")
                .replace("END PUBLIC KEY", "")
            val keyBytes = Base64.getDecoder().decode(key)
            val spec = X509EncodedKeySpec(keyBytes)
            val kf: java.security.KeyFactory = java.security.KeyFactory.getInstance("RSA")
            return kf.generatePublic(spec)
        }
    }

    //end region

    //region responses

    fun data(bytes :ByteArray, mimeType :String) : Response {   
        return newFixedLengthResponse(
            Response.Status.OK,
            mimeType,
            ByteArrayInputStream(bytes),
            bytes.size.toLong())
    }

    private fun notFound(msg: String): Response {
        return newFixedLengthResponse(
            Response.Status.NOT_FOUND,
            "text/plain",
            msg
        )
    }

    private fun badRequest(msg :String) =
        text(msg, Response.Status.BAD_REQUEST)

    private fun internalError(ex :Exception): Response {
        val str = StringBuilder()
        str.appendLine(ex::class.simpleName + ": " + ex.message)

        for (trace in ex.stackTrace) {
            str.appendLine("${trace.className}.${trace.methodName}, line ${trace.lineNumber}")
        }

        return text(str.toString(), Response.Status.INTERNAL_ERROR)
    }

    private fun text(text :String, httpStatus :Response.Status = Response.Status.OK) =
        newFixedLengthResponse(httpStatus, "text/plain", text)

    fun json(json :String)  : Response {
        return newFixedLengthResponse(Response.Status.OK, "application/json", json)
    }

    //endregion

}
