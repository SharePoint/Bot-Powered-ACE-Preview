// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { PropertyPaneChoiceGroupIconProperties } from './propertyPaneChoiceGroupIconProperties';
import { PropertyPaneChoiceGroupImageSize } from './propertyPaneChoiceGroupImageSize';

/**
 * Sharepoint PropertyPaneChoiceGroupOption object
 */
export class PropertyPaneChoiceGroupOption {
    private ariaLabel: string;
    private disabled: boolean;
    private checked: boolean;
    private iconProps: PropertyPaneChoiceGroupIconProperties;
    private imageSize: PropertyPaneChoiceGroupImageSize;
    private imageSrc: string;
    private key: string;
    private text: string;

    /**
     * Initializes a new instance of the PropertyPaneChoiceGroupOption class
     */
    public PropertyPaneChoiceGroupOption() {
        // Do nothing
    }

    /**
     * Sets the aria label of type string
     */
    public set AriaLabel(ariaLabel: string) {
        this.ariaLabel = ariaLabel;
    }

    /**
     * Gets the aria label of type string
     */
    public get AriaLabel(): string {
        return this.ariaLabel;
    }

     /**
     * Sets a value indicating whether this control is enabled or not of type boolean
     */
     public set Disabled(disabled: boolean) {
        this.disabled = disabled;
    }

    /**
     * Gets a value indicating whether this control is enabled or not of type boolean
     */
    public get Disabled(): boolean {
        return this.disabled;
    }

    /**
     * Sets a value indicating whether the property pane group option is checked or not of type boolean
     */
    public set Checked(checked: boolean) {
        this.checked = checked;
    }

    /**
     * Gets a value indicating whether the property pane group option is checked or not of type boolean
     */
    public get Checked(): boolean {
        return this.checked;
    }

    /**
     * Sets the Icon component props for choice field of type PropertyPaneChoiceGroupIconProperties
     */
    public set IconProps(iconProps: PropertyPaneChoiceGroupIconProperties) {
        this.iconProps = iconProps;
    }

    /**
     * Gets the Icon component props for choice field of type PropertyPaneChoiceGroupIconProperties
     */
    public get IconProps(): PropertyPaneChoiceGroupIconProperties {
        return this.iconProps;
    }

    /**
     * Sets the width and height of the image in px for choice field of type PropertyPaneChoiceGroupImageSize
     */
     public set ImageSize(imageSize: PropertyPaneChoiceGroupImageSize) {
        this.imageSize = imageSize;
    }

    /**
     * Gets the width and height of the image in px for choice field of type PropertyPaneChoiceGroupImageSize
     */
    public get ImageSize(): PropertyPaneChoiceGroupImageSize {
        return this.imageSize;
    }

    /**
     * Sets the src of image for choice field of type string
     */
     public set ImageSrc(imageSrc: string) {
        this.imageSrc = imageSrc;
    }

    /**
     * Gets the src of image for choice field of type string
     */
    public get ImageSrc(): string {
        return this.imageSrc;
    }

    /**
     * Sets a key to uniquely identify this option of type string
     */
    public set Key(key: string) {
        this.key = key;
    }

    /**
     * Gets a key to uniquely identify this option of type string
     */
    public get Key(): string {
        return this.key;
    }

    /**
     * Sets text to render for this option of type string
     */
    public set Text(text: string) {
        this.text = text;
    }

    /**
     * Gets text to render for this option of type string
     */
    public get Text(): string {
        return this.text;
    }
}