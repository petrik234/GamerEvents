﻿using System;
using System.Threading.Tasks;

namespace Xamarin_FacebookAuth.Authentication
{
    public interface IFacebookAuthenticationDelegate
    {
        void OnAuthenticationCompleted(FacebookOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}