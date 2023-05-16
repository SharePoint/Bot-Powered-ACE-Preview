// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint GetLocationActionParameters object for get location action
 */
export class GetLocationActionParameters {
    private chooseLocationOnMap: boolean;
    
    /**
     * Initializes a new instance of the GetLocationActionParameters class
     */
    public GetLocationActionParameters() {
        // Do nothing
    }

    /**
     * Sets whether a location on the map can be chosen of type boolean
     */
    public set ChooseLocationOnMap(chooseLocationOnMap: boolean) {
        this.chooseLocationOnMap = chooseLocationOnMap;
    }

    /**
     * Gets whether a location on the map can be chosen of type boolean
     */
    public get ChooseLocationOnMap(): boolean {
        return this.chooseLocationOnMap;
    }
}