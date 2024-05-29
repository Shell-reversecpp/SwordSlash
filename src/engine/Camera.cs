#region Includes

using Microsoft.Xna.Framework;


using Microsoft.Xna.Framework.Graphics;

#endregion

namespace swordSlash;
public class Camera
{
    private Vector2 position;
    private Viewport viewport;
    private Vector2 origin;

    private float radius;
    private float zoom;
    private float rotation;


  public Camera(Viewport viewport, float radius)
{
    this.viewport = viewport;
    origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
    zoom = 1f;
    rotation = 0f;
    this.radius = radius;
}

    public Matrix TransformMatrix => Matrix.CreateTranslation(new Vector3(-position, 0)) *
                                      Matrix.CreateTranslation(new Vector3(-origin, 0)) *
                                      Matrix.CreateRotationZ(rotation) *
                                      Matrix.CreateScale(zoom, zoom, 1) *
                                      Matrix.CreateTranslation(new Vector3(origin, 0));


public void Follow(Player player, int screenWidth)
{
    // Calculate the desired position of the camera
    float playerX = player.GetPosition().X;
    float desiredX = playerX - origin.X;

    // Limit the camera position within the bounds of the world
    float minX = radius; // Minimum X-coordinate
    float maxX = screenWidth - radius; // Maximum X-coordinate

    // Clamp the desired camera position within the world bounds
    float clampedX = MathHelper.Clamp(desiredX, minX, maxX);

    // Smoothly interpolate the camera position towards the desired position
    position.X = MathHelper.Lerp(position.X, clampedX, 0.1f);
}
public Vector2 GetPosition()
    {
        return position;
    }
public Matrix GetMatrix()
{
    // Возвращаем текущую матрицу трансформации камеры
    return Matrix.CreateTranslation(new Vector3(-position, 0)) *
           Matrix.CreateTranslation(new Vector3(-origin, 0)) *
           Matrix.CreateRotationZ(rotation) *
           Matrix.CreateScale(zoom, zoom, 1) *
           Matrix.CreateTranslation(new Vector3(origin, 0));
}
    public void ZoomIn()
    {
        // Увеличение масштаба камеры
        zoom += 2f;
        ClampZoom();
    }
    
     public Matrix GetTransformation()
    {
        // Create the transformation matrix for the camera
        return Matrix.CreateTranslation(new Vector3(-position, 0)) *
               Matrix.CreateTranslation(new Vector3(-origin, 0)) *
               Matrix.CreateRotationZ(rotation) *
               Matrix.CreateScale(zoom, zoom, 1) *
               Matrix.CreateTranslation(new Vector3(origin, 0));
    }
    public void ZoomOut()
    {
        // Уменьшение масштаба камеры
        zoom -= 0.1f;
        ClampZoom();
    }

    private void ClampZoom()
    {
        // Ограничение масштаба камеры
        zoom = MathHelper.Clamp(zoom, 0.1f, 2.0f);
    }
}