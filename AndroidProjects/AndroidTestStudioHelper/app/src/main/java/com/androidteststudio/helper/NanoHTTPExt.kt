package com.androidteststudio.helper

import fi.iki.elonen.NanoHTTPD

fun NanoHTTPD.Response.mimeType(mimeType :String): NanoHTTPD.Response {
    this.mimeType = mimeType;
    return this
}

fun NanoHTTPD.Response.downloadAs(filename :String): NanoHTTPD.Response {
    this.addHeader("Content-Disposition", "attachment; filename=\"${filename}\"")
    return this
}
