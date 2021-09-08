package com.androidteststudio.helper

fun<T> T.flatten(getChildren: (T?) -> List<T?>?): List<T?> {
    val list = mutableListOf<T?>()
    var add :(T) -> Unit = {}
    add = {
        list.add(it)
        for (child in getChildren(it)?: emptyList()) {
            add(it)
        }
    }
    return list
}

/***
 * Returns true if an int has a bitwise flag set.
 */
fun Int.isSet(flag :Int): Boolean {
    return (this and flag) == flag
}