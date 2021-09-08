using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Inventory
{
    private Tool[] equipments;

    public Inventory()
    {
        int toolTypes = Enum.GetNames(typeof(ToolType)).Length;
        this.equipments = new Tool[toolTypes]; 
    }
    /// <summary>
    /// Equip the item. If the item slot is used return true and the old tool, otherwise return false
    /// </summary>
    public bool Equip(Tool item, out Tool old)
    {
        ToolType type = item.GetToolType();
        old = equipments[(int) type];
        equipments[(int) type] = (Tool) item.GetItemPref();
        return !ReferenceEquals(old, null);
    }

    public Tool GetTool(ToolType type)
    {
        return equipments[(int)type];
    }
}

