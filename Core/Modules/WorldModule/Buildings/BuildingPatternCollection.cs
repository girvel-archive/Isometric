using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Isometric.CommonStructures;

namespace Isometric.Core.Modules.WorldModule.Buildings
{
    public class BuildingPatternCollection : IList<BuildingPattern>
    {
        public int LastId { get; private set; } = -1;

        private readonly List<BuildingPattern> _buildingPatterns;



        public BuildingPatternCollection()
        {
            _buildingPatterns = new List<BuildingPattern>();
        }

        public BuildingPatternCollection(IEnumerable<BuildingPattern> patterns)
        {
            _buildingPatterns = new List<BuildingPattern>(patterns);
            LastId = _buildingPatterns.Max(pattern => pattern.Id);
        }



        public BuildingPattern NewPattern(
            string name, 
            Resources resources = new Resources(), 
            Resources price = new Resources(), 
            TimeSpan upgradeTimeNormal = new TimeSpan(), 
            BuildingType type = BuildingType.Nature)
        {
            if (_buildingPatterns.Any(pattern => pattern.Name == name))
            {
                throw new ArgumentException("Pattern with this name already exists", nameof(name));
            }

            var result = new BuildingPattern(name, resources, price, upgradeTimeNormal, type) {Id = ++LastId};
            _buildingPatterns.Add(result);

            return result;
        }

        public BuildingPattern Get(int id)
        {
            return _buildingPatterns[id];
        }

        public BuildingPattern Get(string name)
        {
            return _buildingPatterns.First(p => p.Name == name);
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
        
        public void Add(BuildingPattern item)
        {
            if (_buildingPatterns.Contains(item))
            {
                throw new ArgumentException("Collection already contains this item", nameof(item));
            }

            _buildingPatterns.Add(item);
            item.Id = ++LastId;
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

        public override string ToString() => $"{typeof (BuildingPatternCollection).Name}; LastId: {LastId}";
    }
}