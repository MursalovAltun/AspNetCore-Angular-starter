using System;

namespace Common.GuidProvider
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}