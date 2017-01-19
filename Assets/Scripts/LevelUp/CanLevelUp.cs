namespace SexyBackPlayScene
{
    // 레벨업이 가능한 클래스, 레벨업ui에 표시되기위해 필요한정보도 필수적으로가져야한다.
    public interface CanLevelUp
    {
        string BASEDPC { get; }
        BigInteger BASEDPS { get;  }
        BigInteger DPC { get;  }
        BigInteger DPS { get;  }
        string LEVEL { get; }
        string NEXTDPC { get;  }
        BigInteger NEXTDPS { get;  }
        BigInteger NEXTEXP { get;  }
        BigIntExpression NEXTEXPSTR { get;  }
    }
}