using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpProvider : MySystem
{
    [SerializeField]
    private List<PowerCard> _allPowerUpDatas = new List<PowerCard>();
    
    public List<PowerCard> GetPowerUpDatas => _allPowerUpDatas;

    public List<PowerCard> CopyPowerUpDatas()
    {
        List<PowerCard> copiedList = new List<PowerCard>(_allPowerUpDatas);
        return copiedList;
    }
}
