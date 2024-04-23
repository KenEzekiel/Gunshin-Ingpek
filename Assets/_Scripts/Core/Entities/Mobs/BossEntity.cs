using System.Collections.Generic;
using UnityEngine;

public abstract class BossEntity : EnemyEntity, IAccompaniable
{
    // Attributes
    public List<Companion> companionList = new();    
    public List<bool> companionActive = new();    
    private int companionSelectorIndex;
    
    // Set-Getters
    public List<Companion> CompanionList => companionList;
    public List<bool> CompanionActive => companionActive;
    public MonoBehaviour CompanionController => this;
    public int CompanionSelectorIndex
    {
        get => companionSelectorIndex;
        set 
        {
            // Switch requires a constant, so can't use that here
            if(value == companionList.Count) companionSelectorIndex = 0;
            else if(value == -1) companionSelectorIndex = companionList.Count - 1;
            else if(-1 < value && value < companionList.Count) companionSelectorIndex = value;
            else companionSelectorIndex = 0;
        } 
    }
    
    // Functions
    public void ActivateCompanion(int index)
    {
        if(companionList.Count == 0)
        {
            return;
        }
        CompanionSelectorIndex = index;
        Companion selectedCompanion = CompanionList[CompanionSelectorIndex];
        
        if(CompanionActive[CompanionSelectorIndex])
        {
            return;
        }

        Debug.Log($"Activating Companion {selectedCompanion.name}");
        
        // To handle prefabs
        if(!selectedCompanion.gameObject.scene.IsValid())
        {
            selectedCompanion = ObjectFactory.CreateObject<Companion>(
                prefabPath: selectedCompanion.data.prefabPath,
                parent: EntityManager.Instance.transform, 
                objectName: selectedCompanion.name
            );
            CompanionList[CompanionSelectorIndex] = selectedCompanion;
        }

        selectedCompanion.gameObject.SetActive(true);
        
        // TODO: Set possible spawn locations for companions, for now it will spawn on the left
        selectedCompanion.transform.position = transform.position - new Vector3(-0.5f, 0, 0);
        companionActive[CompanionSelectorIndex] = true;
    }

    public void DeactivateCompanion(int index)
    {
        if(companionList.Count == 0)
        {
            return;
        }
        CompanionSelectorIndex = index;
        Companion selectedCompanion = CompanionList[CompanionSelectorIndex];
        companionActive[CompanionSelectorIndex] = false;
        selectedCompanion.gameObject.SetActive(false);

        Debug.Log($"Deactivating Companion {selectedCompanion.name}");
    }

    public void AddCompanion(Companion companion)
    {
        CompanionList.Add(companion);
        CompanionActive.Add(false);
    }

    public void DeleteCompanion(int index)
    {
        CompanionActive.RemoveAt(index);
        CompanionList.RemoveAt(index);
    }
}
