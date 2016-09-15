using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Isometric.CommonStructures;

namespace Isometric.Core.Modules.WorldModule.Buildings
{
    public class BuildingPatternCollection : IList<BuildingPattern>
    {
        private int _lastId = -1;

        private readonly List<BuildingPattern> _buildingPatterns;



        public BuildingPatternCollection()
        {
            _buildingPatterns = new List<BuildingPattern>();
        }

        public BuildingPatternCollection(IEnumerable<BuildingPattern> patterns)
        {
            patterns = new List<BuildingPattern>(patterns);
        }



        public BuildingPattern NewPattern(
            string name, Resources resources, Resources price, TimeSpan upgradeTimeNormal, 
            BuildingType type = BuildingType.Nature)
        {
            return new BuildingPattern(name, resources, price, upgradeTimeNormal, type) { Id = ++_lastId};
        }
        


        #region IList

        public int Count => _buildingPatterns.Count;

        public bool IsReadOnly => ((IList<BuildingPattern>)_buildingPatterns).IsReadOnly;

        public BuildingPattern this[int index]
        {
            get
            {
                return _buildingPatterns[index];
            }

            set
            {
                _buildingPatterns[index] = value;
            }
        }

        public int IndexOf(BuildingPattern item)
        {
            return _buildingPatterns.IndexOf(item);
        }

        public void Insert(int index, BuildingPattern item)
        {
            _buildingPatterns.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _buildingPatterns.RemoveAt(index);
        }

        [Obsolete("not supported", true)]
        public void Add(BuildingPattern item)
        {
            throw new InvalidOperationException("not supported");
        }

        public void Clear()
        {
            _buildingPatterns.Clear();
        }

        public bool Contains(BuildingPattern item)
        {
            return _buildingPatterns.Contains(item);
        }

        public void CopyTo(BuildingPattern[] array, int arrayIndex)
        {
            _buildingPatterns.CopyTo(array, arrayIndex);
        }

        public bool Remove(BuildingPattern item)
        {
            return _buildingPatterns.Remove(item);
        }

        public IEnumerator<BuildingPattern> GetEnumerator()
        {
            return _buildingPatterns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _buildingPatterns.GetEnumerator();
        }

        #endregion
    }
}