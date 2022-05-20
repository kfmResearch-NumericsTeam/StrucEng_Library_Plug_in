using System.Collections.Generic;

namespace StrucEngLib.Model
{
    public interface Load
    {
        List<Layer> Layers { get; set; }
        LoadType LoadType { get; }
    }
}