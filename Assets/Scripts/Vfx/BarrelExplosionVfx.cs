namespace Vfx
{
    public class BarrelExplosionVfx : BaseVfx
    {
        // Used in animation event
        public void Release()
        {
            Remove();
        }
    }
}
