// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';
import { PropertyPaneChoiceGroupOption } from './propertyPaneChoiceGroupOption';

/**
 * Sharepoint PropertyPaneChoiceGroupProperties object
 */
export class PropertyPaneChoiceGroupProperties implements IPropertyPaneFieldProperties {
    private label: string;
    private options: [PropertyPaneChoiceGroupOption];
    
    /**
     * Initializes a new instance of the PropertyPaneChoiceGroupProperties class
     */
    public PropertyPaneChoiceGroupProperties() {
        // Do nothing
    }

    /**
     * Sets the label of type string
     */
    public set Label(label: string) {
        this.label = label;
    }

    /**
     * Gets the label of type string
     */
    public get Label(): string {
        return this.label;
    }

    /**
     * Sets the collection of options for this choice group of type [PropertyPaneChoiceGroupOption]
     */
     public set Options(options: [PropertyPaneChoiceGroupOption]) {
        this.options = options;
    }

    /**
     * Gets the collection of options for this choice group of type [PropertyPaneChoiceGroupOption]
     */
    public get Options(): [PropertyPaneChoiceGroupOption] {
        return this.options;
    }
}