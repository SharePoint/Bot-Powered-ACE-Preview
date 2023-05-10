// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint Location object
 */
export class Location {
    private latitude: number;
    private longitude: number;
    private timestamp?: number;
    private accuracy?: number;
    
    /**
     * Initializes a new instance of the Location class
     */
    public Location() {
        // Do nothing
    }

    /**
     * Sets latitude property of type number
     */
    public set Latitude(latitude: number) {
        this.latitude = latitude;
    }

    /**
     * Gets latitude property of type number
     */
    public get Latitude(): number {
        return this.latitude;
    }

    /**
     * Sets longitude property of type number
     */
    public set Longitude(longitude: number) {
        this.longitude = longitude;
    }

    /**
     * Gets longitude property of type number
     */
    public get Longitude(): number {
        return this.longitude;
    }

    /**
     * Sets timestamp property of type number
     */
    public set Timestamp(timestamp: number) {
        this.timestamp = timestamp;
    }

    /**
     * Gets timestamp property of type number
     */
    public get Timestamp(): number {
        return this.timestamp;
    }

    /**
     * Sets accuracy property of type number
     */
    public set Accuracy(accuracy: number) {
        this.accuracy = accuracy;
    }

    /**
     * Gets accuracy property of type number
     */
    public get Accuracy(): number {
        return this.accuracy;
    }
}