package com.androidteststudio.helper

import android.graphics.Rect
import kotlin.math.max
import kotlin.math.min

class RectResponse {

    val x :Int
    val y :Int
    val width :Int
    val height :Int

    constructor(rect :Rect) {
        x = min(rect.left, rect.right)
        width = max(rect.left, rect.right) - x
        y = min(rect.top, rect.bottom)
        height = max(rect.top, rect.bottom) - y
    }

}