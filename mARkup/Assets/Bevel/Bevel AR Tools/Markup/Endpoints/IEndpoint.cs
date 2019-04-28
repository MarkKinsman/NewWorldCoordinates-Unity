using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEndpoint
{
    string GetURL();
    WWWForm CreateWWWForm();
    List<IEndpoint> Parse();
}
