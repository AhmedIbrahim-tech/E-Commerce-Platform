"use client";
import React from "react";
import {
  CardContent,
  Typography,
  Avatar,
  Divider,
  Button,
} from "@mui/material";
import ParentCard from "../../shared/ParentCard";
import { Box, Stack } from "@mui/system";
import { IconMessage, IconVolume } from "@tabler/icons-react";

import Slider from "@mui/material/Slider";
import Switch from "@mui/material/Switch";

import ChildCard from "../../shared/ChildCard";

const Settings = () => {
  const [value3, setValue3] = React.useState<number>(45);

  const handleChange6 = (event: Event, newValue: number | number[]) => {
    if (Array.isArray(newValue)) {
      setValue3(newValue[0]);
    } else {
      setValue3(newValue);
    }
  };

  return (
    <ChildCard title="Settings">
      <CardContent sx={{ p: "30px" }}>
        <Typography variant="h5">Settings</Typography>
        <Stack spacing={2} mt={3}>
          <Stack direction="row" spacing={2}>
            <Avatar
              variant="rounded"
              sx={{ bgcolor: "primary.main", width: 48, height: 48 }}
            >
              <IconVolume width={22} />
            </Avatar>
            <Box width="100%">
              <Box
                display="flex"
                alignItems="center"
                justifyContent="space-between"
              >
                <Typography variant="h6">Sound</Typography>
                <Typography variant="subtitle2" color="textSecondary">
                  45%
                </Typography>
              </Box>
              <Slider
                aria-label="Volume"
                value={value3}
                onChange={handleChange6}
              />
            </Box>
          </Stack>
          <Divider />
          <Stack direction="row" spacing={2}>
            <Avatar
              variant="rounded"
              sx={{ bgcolor: "secondary.main", width: 48, height: 48 }}
            >
              <IconMessage width={22} />
            </Avatar>
            <Box
              display="flex"
              alignItems="center"
              justifyContent="space-between"
              width="100%"
            >
              <Box>
                <Typography variant="h6" mb={1}>
                  Chat
                </Typography>
                <Typography variant="subtitle2" color="textSecondary">
                  Turn on chat during call
                </Typography>
              </Box>
              <Box>
                <Switch />
              </Box>
            </Box>
          </Stack>
          <Divider />
        </Stack>
        <Stack direction="row" justifyContent="end" spacing={2} mt={2}>
          <Button variant="outlined" color="error">
            Cancel
          </Button>
          <Button variant="contained" color="primary">
            Save
          </Button>
        </Stack>
      </CardContent>
    </ChildCard>
  );
};

export default Settings;
