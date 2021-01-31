using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace StatMod.Stat
{

    [Serializable] // can edit from unity inspector
    public class GunStat
    {
        public float BaseValue;

        protected float _value;
        protected bool isRemoved = true; // for stop CalculaateValue start every Update();
        protected float lastBaseValue;
        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> _StatModifiers;
        public virtual float Value
        {
            get
            {
                if (isRemoved || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isRemoved = false;
                }
                return _value;
            }
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isRemoved = true;
            statModifiers.Add(mod);
            statModifiers.Sort();
        }
        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isRemoved = true;
                return true;
            }
            return false;
        }
        public virtual bool RemoveAllModifiersFromSource(object source)
        {

            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--)
            // for문을 거꾸로 하는 이유는 정석대로 했을때와 비교해보자
            // 만약 오브젝트의 리스트의 첫번쨰, index 0을 비울때 index 1이 0으로 가고, 2가 1로 가는 등등 비효율적임
            // 만약 오브젝트의 리스트의 마지막을 index 10을 비우면 index의 아무런 일이 생겨나지 않음
            // 그래서 최적화 이유로 거꾸로 하는거
            {
                if (statModifiers[i].Source == source)
                {
                    isRemoved = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }


        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order) { return -1; }
            else if (a.Order > b.Order) { return 1; }
            return 0;
        }

        public GunStat(float baseValue)
        {
            BaseValue = baseValue;
            statModifiers = new List<StatModifier>();
            _StatModifiers = statModifiers.AsReadOnly();
        }


        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0; // hold the sum of Our "PrecentAdd" modifiers

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];
                if (mod.modType == StatModType.Flat)
                {
                    finalValue += mod.Value;
                    /* 예시 = 만약 10%을 더하려고 할떄, 오리지널 값이 100%이니
                     * 1 + 0.1 = 1.1 ,  10 * 1.1 = 11 = 110%
                     */
                }
                else if (mod.modType == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value; // start adding together all modifier of percentAdd Type
                                                // if end of list or next modifier isn't percentadd Type
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].modType != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd; // multuply the sum
                        sumPercentAdd = 0; // Reset the sum back to 0
                    }
                }
                else if (mod.modType == StatModType.PrecentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
            // change 12.023f to 12f
            return (float)Math.Round (finalValue, 4);
        }
    }

}
