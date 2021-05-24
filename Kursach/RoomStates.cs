using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    class RoomStates
    {
        internal enum RoomState
        {
            Unknown,
            Placed,
            NotPlaced,
            NotEnclosed,
            Redundant
        }

        internal double sum = 0.0;
        public RoomState DistinguishRoom(Room room)
        {
            RoomState res = RoomState.Unknown;

            if (room.Area > 0)
            {
                sum += room.Area;
                res = RoomState.Placed;
            }
            else if (null == room.Location)
            {

                res = RoomState.NotPlaced;
            }
            else
            {

                SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();

                IList<IList<BoundarySegment>> segs = room.GetBoundarySegments(opt);

                res = (null == segs || segs.Count == 0)
                  ? RoomState.NotEnclosed
                  : RoomState.Redundant;
            }
            return res;
        }
    }
}
