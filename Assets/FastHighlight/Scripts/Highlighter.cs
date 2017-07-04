using UnityEngine;
using System;
using System.Collections.Generic;
using Merlin;

namespace Merlin.FastHighlight
{
    /// <summary>
    /// A simple and fast component which renders a highlight outline around any object on the scene.
    /// Just drop it on any game object and call one of its method for turning it on.
    /// </summary>
    [ExecuteInEditMode]
    public class Highlighter : MonoBehaviour
    {
        private const float DefaultHighlightThickness = 0.05f;
        private const int StencilRefValue = 187;
        private const string FluidGeometryKeyword = "FLUID_GEOMETRY";

        [SerializeField]
        private bool shouldUpdate = true;

        [SerializeField]
        private bool isFluidGeometry;

        [SerializeField]
        private Color highlightColor;

        [SerializeField]
        private float highlightThickness;

        [SerializeField]
        private Renderer[] renderersToHighlight;

        [SerializeField]
        private Material highlightMaterial;

        [SerializeField]
        private Material stencilBufferMaterial;

        /// <summary>
        /// Gets or sets the color of the highlight outline.
        /// </summary>
        public Color HighlightColor
        {
            get 
            { 
                return this.highlightColor; 
            }
            set
            {
                if (this.highlightColor != value)
                {
                    this.highlightColor = value;
                    this.shouldUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the highlight outline.
        /// </summary>
        public float HighlightThickness
        {
            get 
            { 
                return this.highlightThickness; 
            }
            set
            {
                if (!this.highlightThickness.ApproximatelyEquals(value))
                {
                    this.highlightThickness = value;
                    this.shouldUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the model being highlighted contains
        /// a lot of fluid geometries.
        /// </summary>
        public bool IsFluidGeometry
        {
            get
            {
                return this.isFluidGeometry;
            }
            set
            {
                if (this.isFluidGeometry != value)
                {
                    this.isFluidGeometry = value;
                    this.UpdateGeometryType();
                }
            }
        }

        private Material HighlightMaterial
        {
            get
            {
                if (this.highlightMaterial == null)
                {
                    this.highlightMaterial = new Material(Shader.Find("Merlin/Highlight"));
                    this.highlightMaterial.SetInt("_StencilRefValue", StencilRefValue);
                }

                return this.highlightMaterial;
            }
        }

        private Material StencilBufferMaterial
        {
            get
            {
                if (this.stencilBufferMaterial == null)
                {
                    this.stencilBufferMaterial = new Material(Shader.Find("Merlin/StencilHighlight"));
                    this.stencilBufferMaterial.SetInt("_StencilRefValue", StencilRefValue);
                }

                return this.stencilBufferMaterial;
            }
        }

        /// <summary>
        /// Turns on the highlighter with a constant color.
        /// </summary>
        /// <param name="color">The color of the highlight outline.</param>
        /// <param name="highlightThickness">(Optional) The thickness of the highlight outline.</param>
        public void ConstantOn(Color color, float highlightThickness = DefaultHighlightThickness)
        {
            this.HighlightColor = color;
            this.HighlightThickness = highlightThickness;
        }

        /// <summary>
        /// Turns off the highlighter.
        /// </summary>
        public void ConstantOff()
        {
            this.HighlightThickness = 0f;
        }

        void Awake()
        {
            this.renderersToHighlight = this.GetComponentsInChildren<Renderer>();
            this.UpdateGeometryType();
        }

        void LateUpdate()
        {
            if (this.shouldUpdate)
            {
                this.shouldUpdate = false;
                this.UpdateHighlight();
            }
        }

        void OnDestroy()
        {
            if (this.highlightMaterial != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(this.highlightMaterial);
#else
                Destroy(this.highlightMaterial);
#endif
            }

            if (this.stencilBufferMaterial != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(this.stencilBufferMaterial);
#else
                Destroy(this.stencilBufferMaterial);
#endif
            }
        }

        private void UpdateHighlight()
        {   
            bool removeHighlight = this.highlightThickness.ApproximatelyEquals(0f);
            
            for (int rendererIndex = 0; rendererIndex < renderersToHighlight.Length; rendererIndex++)
            {
                Renderer rendererToHighlight = renderersToHighlight[rendererIndex];
                if (rendererToHighlight == null)
                {
                    continue;
                }

                if (removeHighlight)
                {
                    rendererToHighlight.sharedMaterials = Array.FindAll(rendererToHighlight.sharedMaterials, element => element != this.highlightMaterial && element != this.stencilBufferMaterial);
                }
                else
                {
                    if (!rendererToHighlight.sharedMaterials.Exists(element => element == this.HighlightMaterial))
                    {
                        List<Material> newMaterials = new List<Material>(rendererToHighlight.sharedMaterials);
                        bool stencilVariantShaderAdded = false;

                        for (int i = 0; i < newMaterials.Count; i++)
                        {
                            if (newMaterials[i].shader.name.StartsWith("Merlin/" + Application.unityVersion))
                            {
                                // we have already added an stencil variant of a built-in shader
                                stencilVariantShaderAdded = true;
                                continue;
                            }

                            Shader stencilVariantShader = Shader.Find("Merlin/" + Application.unityVersion + '/' + newMaterials[i].shader.name);
                            if (stencilVariantShader != null)
                            {
                                Material stencilVariantMaterial = new Material(stencilVariantShader);
                                stencilVariantMaterial.CopyPropertiesFromMaterial(newMaterials[i]);
                                stencilVariantMaterial.SetInt("_StencilRefValue", StencilRefValue);
                                newMaterials[i] = stencilVariantMaterial;
                                stencilVariantShaderAdded = true;
                            }

                            if (stencilVariantShaderAdded)
                            {
                                // we only need one shader which writes to the stencil buffer
                                break;
                            }
                        }

                        if (!stencilVariantShaderAdded)
                        {
                            // there is not a shader of which we have a stencil variant
                            // we must add our own shader which writes to the stencil
                            // buffer at the cost of one more draw call
                            newMaterials.Add(this.StencilBufferMaterial);
                        }

                        newMaterials.Add(this.HighlightMaterial);
                        rendererToHighlight.sharedMaterials = newMaterials.ToArray();
                    }
                }
            }

            this.HighlightMaterial.SetColor("_HighlightColor", this.highlightColor);
            this.HighlightMaterial.SetFloat("_HighlightThickness", this.highlightThickness);
        }

        private void UpdateGeometryType()
        {
            if (isFluidGeometry)
            {
                this.HighlightMaterial.EnableKeyword(FluidGeometryKeyword);
            }
            else
            {
                this.HighlightMaterial.DisableKeyword(FluidGeometryKeyword);
            }
        }
    }
}