using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sort_Simulation {
    public struct SortState {
        public SortStateType type;
        public List<SpecialStateValue> specialStateValues;

        public SortState(SortStateType type, List<SpecialStateValue> specialStateValues = null) {
            this.type = type;
            this.specialStateValues = specialStateValues;
        }
    }

    public struct SpecialStateValue {
        public int index;
        public int value;
        public Color color;

        public SpecialStateValue(int index, int value, Color color) {
            this.index = index;
            this.value = value;
            this.color = color;
        }
    }

    public enum SortStateType {
        None,
        Start,
        Swap,
        Compare,
        Sorted
    }
}
