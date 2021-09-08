package com.androidteststudio.helper

import org.joda.time.Instant
import org.joda.time.format.DateTimeFormat

data class ViewChangeResult(
    val changeTypes: List<String>,
    val viewResponses: Map<CharSequence?, ViewResponse>,
    val eventTime: Long,
    val eventTimeFormatted:String = format(eventTime)
){

    companion object {
        fun format(millis :Long):String {
            val time = Instant.ofEpochMilli(millis)
            return DateTimeFormat.forPattern("yyyy-MM-dd HH:mm.ss")
                .print(time.toDateTime())
        }
    }

}