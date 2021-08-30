using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IInteractable
{
    public void Interact(Ninja source);

    public bool CanInteract();

    public string GetDescription();
}
