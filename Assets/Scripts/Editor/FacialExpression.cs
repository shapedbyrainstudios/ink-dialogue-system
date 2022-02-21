using System;
using UnityEngine;

/// <summary>
/// A data structure designed
/// to represent the concept of a facial expression,
/// consisting of an expression name and a Sprite.
/// It is used for grouping the names and faces for
/// the Facial Animation Generator. 
/// </summary>
[Serializable]
public class FacialExpression
{
  public Sprite FacialSprite;

  public String ExpressionName;
}

