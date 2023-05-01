// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';
import { PropertyPaneLinkPopupWindowProperties } from './propertyPaneLinkPopupWindowProperties';

/**
 * Sharepoint PropertyPaneLinkProperties object
 */
export class PropertyPaneLinkProperties implements IPropertyPaneFieldProperties {
    private text: string;
    private target: string;
    private href: string;
    private ariaLabel: string;
    private disabled: boolean;
    private popupWindowProps: PropertyPaneLinkPopupWindowProperties;
    
    /**
     * Initializes a new instance of the PropertyPaneLinkProperties class
     */
    public PropertyPaneLinkProperties() {
        // Do nothing
    }

    /**
     * Sets the label to display next to the checkbox of type string
     */
    public set Text(text: string) {
        this.text = text;
    }

    /**
     * Gets the label to display next to the checkbox of type string
     */
    public get Text(): string {
        return this.text;
    }

    /**
     * Sets where to display the linked resource of type string
     */
    public set Target(target: string) {
        this.target = target;
    }

    /**
     * Gets where to display the linked resource of type string
     */
    public get Target(): string {
        return this.target;
    }

    /**
     * Sets the location to which the link is targeted to of type string
     */
    public set Href(href: string) {
        this.href = href;
    }

    /**
     * Gets the location to which the link is targeted to of type string
     */
    public get Href(): string {
        return this.href;
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
     * Sets the title of pop up window of type string
     */
    public set PopupWindowProps(popupWindowProps: PropertyPaneLinkPopupWindowProperties ){
        this.popupWindowProps = popupWindowProps;
    }

    /**
     * Gets the title of pop up window of type string
     */
    public get PopupWindowProps(): PropertyPaneLinkPopupWindowProperties {
        return this.popupWindowProps;
    }
}