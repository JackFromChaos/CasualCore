using System;


  [Serializable]
  public struct Pair<TKey, TValue>
  {
    public TKey Key;
    public TValue Value;

    public Pair(TKey Key, TValue Value)
    {
      this.Key = Key;
      this.Value = Value;
    }
  }
