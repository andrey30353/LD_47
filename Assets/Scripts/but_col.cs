using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class but_col : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
 
    private Button but;
    private ColorBlock old;
    public ColorBlock neww;
 
    void Start () {
        but = this.GetComponent<Button>();
        old = but.colors;
    }
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        but.colors = neww;
   }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        but.colors = old; 
    }
}