using System;

namespace AutomationEngine
{
    [Serializable]
    public class ItemsLoaderFactory
    {
        public IItemsLoader GetInstance(object contentSource)
        {
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
            var pluginContentSource = contentSource as PluginContentSource;
            if (pluginContentSource != null)
            {
                return new PluginItemsLoader(pluginContentSource.SourceId);
            }
            throw new Exception("Unknown menu content loader");
        }
    }
}