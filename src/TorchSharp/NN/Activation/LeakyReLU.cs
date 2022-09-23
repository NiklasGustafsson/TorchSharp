// Copyright (c) .NET Foundation and Contributors.  All Rights Reserved.  See LICENSE in the project root for license information.
using System;
using System.Runtime.InteropServices;
using static TorchSharp.torch;

namespace TorchSharp
{
    using Modules;

    namespace Modules
    {
        /// <summary>
        /// This class is used to represent a LeakyReLU module.
        /// </summary>
        public class LeakyReLU : torch.nn.Module
        {
            internal LeakyReLU(IntPtr handle, IntPtr boxedHandle) : base(handle, boxedHandle) { }

            [DllImport("LibTorchSharp")]
            private static extern IntPtr THSNN_LeakyReLU_forward(torch.nn.Module.HType module, IntPtr tensor);

            public override Tensor forward(Tensor tensor)
            {
                var res = THSNN_LeakyReLU_forward(handle, tensor.Handle);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new Tensor(res);
            }

            public override string GetName()
            {
                return typeof(LeakyReLU).Name;
            }
        }
    }

    public static partial class torch
    {
        public static partial class nn
        {
            [DllImport("LibTorchSharp")]
            extern static IntPtr THSNN_LeakyReLU_ctor(double negative_slope, [MarshalAs(UnmanagedType.U1)] bool inplace, out IntPtr pBoxedModule);

            /// <summary>
            /// Continuously Differentiable Exponential Linear Unit
            /// </summary>
            /// <param name="negative_slope">The α value for the LeakyReLU formulation.</param>
            /// <param name="inplace">Do the operation in-place.</param>
            /// <returns></returns>
            static public LeakyReLU LeakyReLU(double negative_slope = 0.01, bool inplace = false)
            {
                var handle = THSNN_LeakyReLU_ctor(negative_slope, inplace, out var boxedHandle);
                if (handle == IntPtr.Zero) { torch.CheckForErrors(); }
                return new LeakyReLU(handle, boxedHandle);
            }

            public static partial class functional
            {
                /// <summary>
                /// Continuously Differentiable Exponential Linear Unit
                /// </summary>
                /// <param name="input">The input tensor</param>
                /// <param name="negative_slope">The α value for the LeakyReLU formulation. Default: 1.0</param>
                /// <param name="inplace">Do the operation in-place. Default: False</param>
                /// <returns></returns>
                static public Tensor leaky_relu(Tensor input, double negative_slope = 0.01, bool inplace = false)
                {
                    using (var m = nn.LeakyReLU(negative_slope, inplace)) {
                        return m.forward(input);
                    }
                }
            }
        }
    }
}
