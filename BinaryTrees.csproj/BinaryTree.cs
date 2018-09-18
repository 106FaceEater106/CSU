using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<E> : IEnumerable<E> where E : IComparable
    {
        private TreeUnit<E> root;
        
        public void Add(E pass)
        {
            var insertedUnit = new TreeUnit<E>(pass);
            if (root == null)
                root = insertedUnit;
            else
            {
                var currentUnit = root;
                while (true)
                {
                    if (currentUnit.Importance.CompareTo(pass) > 0)
                    {
                        if (currentUnit.Left == null)
                        {
                            currentUnit.Left = new TreeUnit<E>(pass);
                            break;
                        }
                        currentUnit = currentUnit.Left;
                    }
                    else
                    {
                        if (currentUnit.Right == null)
                        {
                            currentUnit.Right = new TreeUnit<E>(pass);
                            break;
                        }
                        currentUnit = currentUnit.Right;
                    }
                }
            }
        }
        
        public bool Contains(E pass)
        {
            var currentUnit = root;
            while (currentUnit != null)
            {
                var diff = currentUnit.Importance.CompareTo(pass);
                if (diff == 0)
                    return true;
                currentUnit = diff > 0 ? currentUnit.Left : currentUnit.Right;
            }
            return false;
        }
        
        public E this[int number]
        {
            get
            {
                if (root == null || number < 0 || number >= root.Amount) throw new IndexOutOfRangeException();
                var currentUnit = root;
                while (true)
                {
                    var amountLeft = currentUnit.Left?.Amount ?? 0;
                    if (number == amountLeft)
                        return currentUnit.Importance;
                    if (number < amountLeft)
                        currentUnit = currentUnit.Left;
                    else
                    {
                        currentUnit = currentUnit.Right;
                        number = number - (1 + amountLeft);
                    }
                }
            }  
        }

        public IEnumerator<E> GetEnumerator()
        {
            return root?.GetImportances().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class TreeUnit<E> where E : IComparable
    {
        public E Importance { get; }
        private TreeUnit<E> superUnit;
        private TreeUnit<E> lft;

        public TreeUnit<E> Left
        {
            get { return lft; }
            set
            {
                if (lft != null)
                    IsFollowingModification(-lft.Amount);              
                lft = value;
                if (value != null)
                {
                    IsFollowingModification(value.Amount);
                    value.superUnit = this;
                }
            }
        }

        private TreeUnit<E> rght;
        public TreeUnit<E> Right
        {
            get { return rght; }
            set
            {
                if (rght != null)
                    IsFollowingModification(-rght.Amount);
                rght = value;
                if (value != null)
                {
                    IsFollowingModification(value.Amount);
                    value.superUnit = this;
                }
            }
        }

        public int Amount { get; private set; }

        public TreeUnit(E importance)
        {
            if (importance == null)
                throw new ArgumentNullException(nameof(importance));
            Importance = importance;
            Amount = 1;
        }

        private void IsFollowingModification(int gap)
        {
            Amount = Amount + gap;
            superUnit?.IsFollowingModification(gap);
        }
        
        public IEnumerable<E> GetImportances()
        {
            if (Left != null)
                foreach (var importance in Left.GetImportances())
                    yield return importance;
            yield return Importance;
            if (Right != null)
                foreach (var importance in Right.GetImportances())
                    yield return importance;
        }
    }
}