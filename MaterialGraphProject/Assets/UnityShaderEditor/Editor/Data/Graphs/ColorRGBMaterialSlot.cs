using System;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph.Drawing.Slots;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ShaderGraph
{
    public class ColorRGBMaterialSlot : Vector3MaterialSlot
    {
        public ColorRGBMaterialSlot() {}

        public ColorRGBMaterialSlot(
            int slotId,
            string displayName,
            string shaderOutputName,
            SlotType slotType,
            Color value,
            ShaderStage shaderStage = ShaderStage.Dynamic,
            bool hidden = false)
            : base(slotId, displayName, shaderOutputName, slotType, (Vector4)value, shaderStage, hidden)
        {
        }

        public override VisualElement InstantiateControl()
        {
            return new ColorRGBSlotControlView(this);
        }

        protected override string ConcreteSlotValueAsVariable(AbstractMaterialNode.OutputPrecision precision)
        {
            return string.Format("IsGammaSpace() ? {0}3({1}, {2}, {3}) : GammaToLinearSpace({0}3({1}, {2}, {3}))"
                , precision
                , value.x
                , value.y
                , value.z);
        }

        public override void AddDefaultProperty(PropertyCollector properties, GenerationMode generationMode)
        {
            if (!generationMode.IsPreview())
                return;

            var matOwner = owner as AbstractMaterialNode;
            if (matOwner == null)
                throw new Exception(string.Format("Slot {0} either has no owner, or the owner is not a {1}", this, typeof(AbstractMaterialNode)));

            var property = new ColorShaderProperty()
            {
                overrideReferenceName = matOwner.GetVariableNameForSlot(id),
                generatePropertyBlock = false,
                value = new Color(value.x, value.y, value.z)
            };
            properties.AddShaderProperty(property);
        }

        public override PreviewProperty GetPreviewProperty(string name)
        {
            var pp = new PreviewProperty
            {
                name = name,
                propType = PropertyType.Color,
                vector4Value = new Vector4(value.x, value.y, value.z, 1),
                floatValue = value.x,
                colorValue = new Vector4(value.x, value.y, value.z, 1),
            };
            return pp;
        }
    }
}
