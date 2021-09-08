package com.androidteststudio.helper

import androidx.annotation.RequiresApi

class BigData {

    @RequiresApi(28)
    fun name() :String { return ""}

    @RequiresApi(29)
    fun type() :String { return ""}

    @RequiresApi(30)
    fun description() :String  { return ""}

    //lots of other fields
}

data class SmallData (
    val name :String,
    val type :String,
    val descriptor :String
) {

    constructor(data :BigData)
        : this(data.name(), data.type(), data.description())

}
