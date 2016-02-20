﻿using System;

namespace AutomationEngine
{
    [Serializable]
    public class ExecutableItemsLoaderFactory
    {
        public IExecutableItemsLoader GetInstance(object contentSource)
        {
            var fileDescriptorContentSource = contentSource as FileDescriptorContentSource;
            if (fileDescriptorContentSource != null)
            {
                return new FileDescriptorItemsLoader(fileDescriptorContentSource.Path);
            }
            var rawFileContentSource = contentSource as RawFileContentsSource;
            if (rawFileContentSource != null)
            {
                return new RawFileItemsLoader(rawFileContentSource);
            }
            throw new Exception("Unknown menu content loader");
        }
    }
}