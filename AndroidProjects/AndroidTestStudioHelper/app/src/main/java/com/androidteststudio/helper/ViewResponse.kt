package com.androidteststudio.helper

import android.os.Build
import android.view.accessibility.AccessibilityNodeInfo
import androidx.annotation.RequiresApi

data class ViewResponse(
    val id :CharSequence?,
    val className :CharSequence?,
    val text :CharSequence?,
    val inputType :Int,
    val isAccessibilityFocused :Boolean,
    val isCheckable :Boolean,
    val isChecked :Boolean,
    val isClickable :Boolean,
    val isContentInvalid :Boolean,
    val isContextClickable :Boolean,
    val isDismissable :Boolean,
    val isEditable :Boolean,
    val isEnabled :Boolean,
    val isHeading :Boolean,
    val contentDescription :CharSequence?,
    val error :CharSequence?,
    val hintText :CharSequence?,
    val maxTextLength :Int,
    val packageName :CharSequence?,
    val isScrollable :Boolean,
    val tooltipText :CharSequence?,
    val boundsInParent : RectResponse,
    val boundsInScreen : RectResponse,
    val paneTitle :CharSequence?,
    val labelForId :CharSequence?,
    val children :List<ViewResponse?>
) {

    @RequiresApi(Build.VERSION_CODES.P)
    constructor(node: AccessibilityNodeInfo)
        : this(
            node.viewIdResourceName,
            node.className,
            node.text?.toString(),
            node.inputType,
            node.isAccessibilityFocused,
            node.isCheckable,
            node.isChecked,
            node.isClickable,
            node.isContentInvalid,
            node.isContextClickable,
            node.isDismissable,
            node.isEditable,
            node.isEnabled,
            node.isHeading,
            node.contentDescription?.toString(),
            node.error,
            node.hintText?.toString(),
            node.maxTextLength,
            node.packageName,
            node.isScrollable,
            node.tooltipText,
            node.getBoundsInParent().let{ RectResponse(it) },
            node.getBoundsInScreen().let{ RectResponse(it) },
            node.paneTitle,
            node.labelFor?.viewIdResourceName,
            node.children().map {
                it?.let { ViewResponse(it) }
            }
        ) {

        }
}