// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

export enum FieldType{
    CheckBox = 2,
    TextField = 3,
    Toggle = 5, 
    Dropdown = 6,
    Label = 7, 
    Slider = 8, 
    ChoiceGroup = 10,
    HorizontalRule = 12,
    Link = 13,
    DynamicField = 14,
    DynamicFieldSet = 16,
    SpinButton = 17,
    ThumbnailPicker = 18,
    IconPicker = 19
}

/**
 * Sharepoint PropertyPaneGroupField object
 */
export class PropertyPaneField{
    private type: FieldType;
    private targetProperty: string; 
    private properties: IPropertyPaneFieldProperties;
    private shouldFocus: boolean;
    /**
     * Initializes a new instance of the PropertyPaneField class
     */
    public PropertyPaneField(){
        // Do nothing
    }

    /**
     * Sets the type of field
     */
    public set Type(type: FieldType){
        this.type = type;
    }

    /**
     * Gets the type of field
     */
    public get Type(): FieldType {
        return this.type;
    }

    /**
     * Sets the target property of type string
     */
    public set TargetProperty(targetProperty: string){
        this.targetProperty = targetProperty;
    }

    /**
     * Gets the target property of type string
     */
    public get TargetProperty(): string {
        return this.targetProperty;
    }

    /**
     * Sets the properties property of type IPropertyPaneFieldProperties
     */
    public set Properties(properties: IPropertyPaneFieldProperties){
        this.properties = properties;
    }

    /**
     * Gets the properties property of type IPropertyPaneFieldProperties
     */
    public get Properties(): IPropertyPaneFieldProperties {
        return this.properties;
    }

    /**
     * Sets a value indicating whether this control should be focused of type boolean
     */
     public set ShouldFocus(shouldFocus: boolean){
        this.shouldFocus = shouldFocus;
    }

    /**
     * Gets a value indicating whether this control should be focused of type boolean
     */
    public get ShouldFocus(): boolean {
        return this.shouldFocus;
    }

}