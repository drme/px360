using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThW.Render.Textures;
using ThW.XNA.Storage;

namespace ThW.X360.Controller.Windows
{
    class XNAContentTextureLoader : XNAContentStoreProvider, ITextureLoader
    {
        public XNAContentTextureLoader(ContentManager contentManager, String contentFolder, String allowedFilesList) : base(contentFolder, allowedFilesList)
        {
            this.loadingTexture = contentManager.Load<Texture2D>("loading");
            this.contentManager = contentManager;
        }

        public ThW.Render.Textures.Texture CreateTexture(String textureName, bool useMipMap, bool wraps, bool wrapt)
        {
            if (true == CanLoadFile(textureName + ".xnb"))
            {
                return new ThW.Render.Textures.Texture(true, textureName, this.loadingTexture, 2560, 2560, this);
            }

            return null;
        }

        public bool LoadTexture(ThW.Render.Textures.Texture texture, ITexture noTexture)
        {
            Texture2D texture2d = this.contentManager.Load<Texture2D>(texture.Name);
            texture.UpdateTexture(texture2d, TextureState.Loaded, texture2d.Width, texture2d.Height);

            return true;
        }

        public void UnLoad(ThW.Render.Textures.Texture texture)
        {
            // those textures are here for all life cycle
        }

        private Texture2D loadingTexture = null;
        private ContentManager contentManager = null;
    }
}
