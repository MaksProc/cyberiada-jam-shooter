public interface IReloadable
{
    int magSize { get; }
    int currentMagAmmo { get; }
    int currentAmmo { get; }
    float reloadTime { get; }
    public void Reload();
}