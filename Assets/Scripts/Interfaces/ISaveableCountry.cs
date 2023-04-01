using Assets;

namespace Interfaces
{
    public interface ISaveableCountry
    {
        void Import(ProgressCountry progressCountry);
        ProgressCountry Export();
    }
}