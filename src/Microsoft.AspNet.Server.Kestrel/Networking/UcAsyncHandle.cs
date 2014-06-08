﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Server.Kestrel.Networking
{
    public class UvAsyncHandle : UvHandle
    {
        private static Libuv.uv_async_cb _uv_async_cb = AsyncCb;

        unsafe static void AsyncCb(IntPtr handle)
        {
            FromIntPtr<UvAsyncHandle>(handle)._callback.Invoke();
        }

        private Action _callback;

        public void Init(UvLoopHandle loop, Action callback)
        {
            CreateHandle(loop, 256);
            _callback = callback;
            _uv.async_init(loop, this, _uv_async_cb);
        }

        public void DangerousClose()
        {
            Close();
            ReleaseHandle();
        }

        private void UvAsyncCb(IntPtr handle)
        {
            _callback.Invoke();
        }

        public void Send()
        {
            _uv.async_send(this);
        }
    }
}
