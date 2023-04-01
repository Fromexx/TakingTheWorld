using Assets;

namespace Interfaces
{
    public interface ISaveableRegion
    {
        void Import(ProgressRegion progressRegion);
        ProgressRegion Export();
    }
}