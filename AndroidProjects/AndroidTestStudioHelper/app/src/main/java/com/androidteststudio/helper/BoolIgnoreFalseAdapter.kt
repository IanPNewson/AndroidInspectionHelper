package com.androidteststudio.helper

import com.google.gson.TypeAdapter
import com.google.gson.stream.JsonReader
import com.google.gson.stream.JsonWriter
import java.io.IOException

class BoolIgnoreFalseAdapter : TypeAdapter<Boolean>() {
    @Throws(IOException::class)
    override fun read(reader: JsonReader): Boolean {throw NotImplementedError()}

    @Throws(IOException::class)
    override fun write(out: JsonWriter, data: Boolean) {
        if (!data) {
            out.nullValue()
            return
        }
        out.value(data)
    }

}