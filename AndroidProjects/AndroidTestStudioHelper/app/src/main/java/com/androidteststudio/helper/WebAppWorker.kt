package com.androidteststudio.helper

import android.content.Context
import android.content.Context.WINDOW_SERVICE
import android.view.WindowManager
import androidx.core.content.ContextCompat.getSystemService
import androidx.work.Worker
import androidx.work.WorkerParameters


class WebAppWorker(context: Context, workerParams: WorkerParameters) : Worker(context, workerParams) {

    private var _app :WebApp? = null/*(Responder(
        this.applicationContext,
        context.getSystemService(Context.WINDOW_SERVICE) as WindowManager
    ), 11023)*/

    override fun doWork(): Result {
        if (!_app!!.isAlive)
            _app!!.start()
        return Result.success()
    }

    override fun onStopped() {
        super.onStopped()
        if (_app!!.isAlive)
            _app!!.stop()
    }

}