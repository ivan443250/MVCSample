using MVCSample.Tools;
using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public interface IModule : IDependencyCollectionElement
    {
        void Initialize(Context context);
    }
}