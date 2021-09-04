package com.androidteststudio.helper

import android.view.View

fun View.show() {
    this.visibility = View.VISIBLE
}

fun View.hide() {
    this.visibility = View.GONE
}

fun View.click(onClick :(() -> Unit)) {
    this.setOnClickListener { onClick() }
}