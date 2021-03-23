// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Runtime.InteropServices;
using TorchSharp.Tensor;

namespace TorchSharp.NN
{
    /// <summary>
    /// This class is used to represent a BatchNorm2D module.
    /// </summary>
    public class BatchNorm2D : Module
    {
        internal BatchNorm2D (IntPtr handle, IntPtr boxedHandle) : base (handle, boxedHandle)
        {
        }

        [DllImport ("LibTorchSharp")]
        private static extern IntPtr THSNN_BatchNorm2d_forward (IntPtr module, IntPtr tensor);

        public TorchTensor forward (TorchTensor tensor)
        {
            var res = THSNN_BatchNorm2d_forward (handle.DangerousGetHandle (), tensor.Handle);
            if (res == IntPtr.Zero) { Torch.CheckForErrors(); }
            return new TorchTensor (res);
        }
    }
    public static partial class Modules
    {
        [DllImport ("LibTorchSharp")]
        extern static IntPtr THSNN_BatchNorm2d_ctor (long features, double eps, double momentum, bool affine, bool track_running_stats, out IntPtr pBoxedModule);

        static public BatchNorm2D BatchNorm2D (long features, double eps = 1e-05, double momentum = 0.1, bool affine = true, bool track_running_stats = true)
        {
            unsafe {
                var handle = THSNN_BatchNorm2d_ctor (features, eps, momentum, affine, track_running_stats, out var boxedHandle);
                if (handle == IntPtr.Zero) { Torch.CheckForErrors(); }
                return new BatchNorm2D (handle, boxedHandle);
            }
        }
    }

    public static partial class Functions
    {
        static public TorchTensor BatchNorm2D (TorchTensor x, long features, double eps = 1e-05, double momentum = 0.1, bool affine = true, bool track_running_stats = true)
        {
            using (var d = Modules.BatchNorm2D (features, eps, momentum, affine, track_running_stats)) {
                return d.forward (x);
            }
        }
    }
}
