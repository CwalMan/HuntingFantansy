namespace StatMod.Stat
{
    public enum StatModType
    {
        Flat = 100, // add +,-
        PercentAdd = 200, // add %
        PrecentMult = 300, // 100% * 100 으로 퍼센트끼리 곱할 때 거듭제곱하여 400% 되는 걸 막는다.
                           // 100,200 등 숫자들은 만약 누군가 어떠한 modifiers를 위해 Order 값을 개조하려 할때 유연성을 높이게한다
                           // 예로 percentAdd와 percentMult 사이에 Flat modifier를 개조하려 할때 order값에서 201~299값 중 하나 뽑아 할 수 있게 한다
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType modType;
        public readonly int Order;
        public readonly object Source;
        /* Order 존재 이유
         * 만약 스킬이나 아이템이 힘을 10% 증가시켜줄 때 아이템이 20+ 힘을 준다하자
         * 그러면 10 * 1.1 + 20 값이 31이 된다.
         * 정상적이라면 (10 + 20 ) * 1.1 = 33 이 되어 본 능력치가 330% 로
         * 따로 () 값을 만들어주는게 Order다.
         */

        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            modType = type;
            Order = order;
            Source = source;
        }


        // Requires Value and Type, Call the 'Main' constructor and Set Order and Source to ther default values :(int) type and null,respectively
        public StatModifier(float value, StatModType type) : this(value, type, (int)type)
        {
        }
        // Requires Value,Type,Order, Set Source to it's default value:null
        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null)
        {
        }
        //Requires Value,Type,Source, Set Order to it's default value: (int)Type
        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source)
        {
        }
    }
}

