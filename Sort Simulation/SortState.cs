using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sort_Simulation {
    public struct SortState {
        public SortStateType type;
        public List<ArrayState> arrayStates;

        public SortState(SortStateType type, List<ArrayState> arrayStates) {
            this.type = type;
            this.arrayStates = arrayStates;
        }
    }

    public struct ArrayState {
        public int[] array;
        public int offset;
        public List<SpecialStateValue> specialStateValues;

        public ArrayState(int[] array, int offset, List<SpecialStateValue> specialStateValues = null) {
            this.array = array;
            this.offset = offset;
            this.specialStateValues = specialStateValues;

            if (specialStateValues == null) {
                this.specialStateValues = new List<SpecialStateValue>();
            }
        }
    }

    public struct SpecialStateValue {
        public int index;
        public Color color;

        public SpecialStateValue(int index, Color color) {
            this.index = index;
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
