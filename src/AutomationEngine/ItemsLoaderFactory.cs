using System;

namespace AutomationEngine
{
    [Serializable]
    public class ItemsLoaderFactory
    {
        public IItemsLoader GetInstance(object contentSource)
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
            var ahkFunctionContentSource = contentSource as AhkFunctionContentsSource;
            if (ahkFunctionContentSource != null)
            {
                return new AhkFunctionItemsLoader(ahkFunctionContentSource);
            }
            throw new Exception("Unknown menu content loader");
        }
    }
}