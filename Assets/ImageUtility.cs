using UnityEngine;

public static class ImageUtility
{
    public static Texture2D CropToSquare(Texture2D originalTexture)
    {
        // Determine the size of the square
        int squareSize = Mathf.Min(originalTexture.width, originalTexture.height);

        // Calculate the starting point for cropping
        int startX = (originalTexture.width - squareSize) / 2;
        int startY = (originalTexture.height - squareSize) / 2;

        // Create a new texture for the cropped image
        Texture2D croppedTexture = new Texture2D(squareSize, squareSize);

        // Copy the pixels from the original image to the new one
        for (int x = 0; x < squareSize; x++)
        {
            for (int y = 0; y < squareSize; y++)
            {
                Color pixel = originalTexture.GetPixel(x, y);
                croppedTexture.SetPixel(x, y, pixel);
            }
        }

        // Apply changes to the cropped texture
        croppedTexture.Apply();

        return croppedTexture;
    }

    public static Sprite TextureToSprite(Texture2D textureToConvert)
    {
        Texture2D croppedTexture = ImageUtility.CropToSquare(textureToConvert);

        // Create a sprite from the cropped texture
        Sprite newSprite = Sprite.Create(croppedTexture, new Rect(0.0f, 0.0f, croppedTexture.width, croppedTexture.height), new Vector2(0.5f, 0.5f));

        return newSprite;
    }
}
