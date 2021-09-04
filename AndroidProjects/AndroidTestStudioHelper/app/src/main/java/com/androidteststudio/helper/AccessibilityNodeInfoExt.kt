package com.androidteststudio.helper

import android.graphics.Rect
import android.view.accessibility.AccessibilityNodeInfo

fun AccessibilityNodeInfo.children(): List<AccessibilityNodeInfo?> {
    val views = mutableListOf<AccessibilityNodeInfo?>()
    for (i in 0 until childCount) {
        val child = getChild(i) //?: return null//I don't know why this null happens, but keeping the old set of views seems more useful
        if (null == child) {
            "".toString()
        }
        child.apply { views.add(child) }
    }
    return views
}


fun AccessibilityNodeInfo.getBoundsInParent() : Rect {
    val rect = Rect()
    getBoundsInParent(rect)
    return rect
}

fun AccessibilityNodeInfo.getBoundsInScreen() : Rect {
    val rect = Rect()
    getBoundsInScreen(rect)
    return rect
}