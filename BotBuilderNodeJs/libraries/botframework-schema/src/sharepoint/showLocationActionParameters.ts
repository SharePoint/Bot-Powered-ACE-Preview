// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ICardActionParameters } from './ICardActionParameters';
import { IOnCardSelectionActionParameters } from './IOnCardSelectionActionParameters';
import { Location } from './location';

/**
 * Sharepoint ShowLocationActionParameters object for show location action
 */
export class ShowLocationActionParameters implements ICardActionParameters, IOnCardSelectionActionParameters {
    private locationCoordinates: Location;
    
    /**
     * Initializes a new instance of the ShowLocationActionParameters class
     */
    public ShowLocationActionParameters() {
        // Do nothing
    }

    /**
     * Sets whether a location on the map can be chosen of type boolean
     */
    public set LocationCoordinates(locationCoordinates: Location) {
        this.locationCoordinates = locationCoordinates;
    }

    /**
     * Gets whether a location on the map can be chosen of type boolean
     */
    public get LocationCoordinates(): Location {
        return this.locationCoordinates;
    }
}